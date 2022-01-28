import React, {useEffect, useState} from 'react';
import {Api} from "../common/Api";
import {Item} from "../models/Item";
import {toast} from "react-toastify";
import ItemsDropdown from "./ItemsDropdown";
import {Columns} from "react-bulma-components";
import ItemInfo from "./ItemInfo";
import ClientInfoInputModal from "./ClientInfoInputModal";
import {ClientInfoRequest} from "../models/ClientInfoRequest";

function ItemsPage(props: any) {
    const [items, setItems] = useState<Item[] | null>()
    const [chosenItem, setChosenItem] = useState<Item | undefined>()
    const [modalOpen, setModalOpen] = useState<boolean>(false)

    const fetchItems = async () => {
        const items = await toast.promise(() => Api.getAvailableItems(), {
            pending: 'Загрузка данных...',
            error: 'При загрузке данных произошла непредвиденная ошибка',
            success: 'Данные загружены'
        })
        setItems(items)
    }

    const handleBuyButtonClick = async () => {
        setModalOpen(true)
    }
    
    const handleClientInfoModalBuyButtonClicked = async (clientInfo: ClientInfoRequest) => {
        if (!chosenItem) {
            toast.warn("Перед совершением оплаты выберите продукт");
            return;
        }

        const resp = await Api.makePayment({
            itemId: chosenItem.id,
            clientInfo: clientInfo
        })

        if (!resp?.url) {
            return;
        }

        window.location.replace(resp.url);
    }

    useEffect(() => {
        void fetchItems()
        document.title = "@stushbrphoto - Вдохновение"
    }, []);

    return (
            <Columns>
                <Columns.Column>
                    {items && <ItemsDropdown items={items!} onDropdownItemChange={setChosenItem} />}
                </Columns.Column>
                {chosenItem && <Columns.Column size="three-quarters">
                  <ItemInfo item={chosenItem} buyButtonClickHandler={handleBuyButtonClick} />
                </Columns.Column>}
                <ClientInfoInputModal
                    isOpen={modalOpen}
                    itemInfo={chosenItem}
                    onModalClose={() => setModalOpen(false)}
                    onBuyButtonClicked={handleClientInfoModalBuyButtonClicked}
                />
            </Columns>
    );
}

export default ItemsPage;