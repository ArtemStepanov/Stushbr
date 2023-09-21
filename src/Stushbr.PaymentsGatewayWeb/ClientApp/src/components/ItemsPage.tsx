import React, {useEffect, useState} from "react";
import {StushbrApi} from "../common/StushbrApi";
import {Item} from "../models/Item";
import {toast} from "react-toastify";
import ItemsDropdown from "./ItemsDropdown";
import {Block, Heading} from "react-bulma-components";
import ItemInfo from "./ItemInfo";
import ClientInfoInputModal from "./ClientInfoInputModal";
import {Defaults} from "../common/Defaults";

function ItemsPage(props: any) {
    const [items, setItems] = useState<Item[] | null>()
    const [chosenItem, setChosenItem] = useState<Item | null>(null)
    const [modalOpen, setModalOpen] = useState<boolean>(false)

    const fetchItems = async () => {
        const items = await toast.promise(() => StushbrApi.getAvailableItems(), Defaults.DefaultToastPromiseMessages, {
            autoClose: 2000,
            pauseOnFocusLoss: false
        })

        setItems(items)
    }

    useEffect(() => {
        void fetchItems()
        document.title = Defaults.Title
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
                <ItemInfo item={chosenItem} buyButtonClickHandler={() => setModalOpen(true)} />
              </Block>
            }
            <ClientInfoInputModal
                isOpen={modalOpen}
                itemInfo={chosenItem}
                onModalClose={() => setModalOpen(false)}
                onBuyButtonClicked={() => setModalOpen(false)}
            />
        </>
    );
}

export default ItemsPage;