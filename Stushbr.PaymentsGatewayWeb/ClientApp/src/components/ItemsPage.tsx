import React, {useEffect, useState} from "react";
import {StushbrApi} from "../common/StushbrApi";
import {Item} from "../models/Item";
import {toast} from "react-toastify";
import ItemsDropdown from "./ItemsDropdown";
import {Block, Heading} from "react-bulma-components";
import ItemInfo from "./ItemInfo";
import ClientInfoInputModal from "./ClientInfoInputModal";
import {ClientInfoRequest} from "../models/ClientInfoRequest";

function ItemsPage(props: any) {
    const [items, setItems] = useState<Item[] | null>()
    const [chosenItem, setChosenItem] = useState<Item | undefined>()
    const [modalOpen, setModalOpen] = useState<boolean>(false)

    const fetchItems = async () => {
        const items = await toast.promise(() => StushbrApi.getAvailableItems(), {
            pending: "Загрузка данных...",
            error: "При загрузке данных произошла непредвиденная ошибка",
            success: "Данные загружены"
        }, {
            autoClose: 2000,
            pauseOnFocusLoss: false
        })
        setItems(items)
    }

    const handleBuyButtonClick = async () => {
        setModalOpen(true)
    }

    const handleClientInfoFormSubmit = async (clientInfo: ClientInfoRequest) => {
        console.log(clientInfo);

        if (!chosenItem) {
            toast.warn("Перед совершением оплаты выберите продукт");
            return;
        }

        const resp = await StushbrApi.makePayment({
            id: chosenItem.id,
            clientInfo: clientInfo
        })

        if (!resp?.url) {
            return;
        }

        window.open(resp.url, "_blank")
        setModalOpen(false)
    }

    useEffect(() => {
        void fetchItems()
        document.title = "@stushbrphoto - Вдохновение"
    }, []);

    return (
        <>
            <Block textAlign="center">
                <Heading>
                    Привет!
                </Heading>
                <Heading subtitle>
                    Добро пожаловать
                </Heading>
            </Block>
            {items &&
              <Block>
                <ItemsDropdown
                  items={items!}
                  onDropdownItemChange={setChosenItem}
                />
              </Block>
            }
            {chosenItem &&
              <Block>
                <ItemInfo item={chosenItem} buyButtonClickHandler={handleBuyButtonClick} />
              </Block>
            }
            <ClientInfoInputModal
                isOpen={modalOpen}
                itemInfo={chosenItem}
                onModalClose={() => setModalOpen(false)}
                onBuyButtonClicked={handleClientInfoFormSubmit}
            />
        </>
    );
}

export default ItemsPage;