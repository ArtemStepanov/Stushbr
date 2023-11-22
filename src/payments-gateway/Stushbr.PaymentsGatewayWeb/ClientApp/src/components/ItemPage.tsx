import {useCallback, useEffect, useState} from 'react';
import {useParams} from "react-router-dom";
import {toast} from "react-toastify";
import {StushbrApi} from "../common/StushbrApi";
import {Defaults} from "../common/Defaults";
import {Item} from "../models/Item";
import {Block, Heading} from "react-bulma-components";
import ItemInfo from "./ItemInfo";
import ClientInfoInputModal from "./ClientInfoInputModal";

function ItemPage(props: any) {
    const [item, setItem] = useState<Item | null>(null)
    const [modalOpen, setModalOpen] = useState<boolean>(false)

    const params = useParams<{ id: string }>()

    const fetchItem = useCallback(async () => {
        const fetchedItem = await toast.promise(() => StushbrApi.getItemById(params.id), Defaults.DefaultToastPromiseMessages)
        setItem(fetchedItem)
    }, [params.id])

    useEffect(() => {
        void fetchItem()
        document.title = Defaults.Title
    }, [fetchItem])

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
            {item &&
              <Block>
                <ItemInfo item={item} buyButtonClickHandler={() => {
                    setModalOpen(true)
                }} />
              </Block>
            }
            <ClientInfoInputModal
                isOpen={modalOpen}
                itemInfo={item}
                onModalClose={() => setModalOpen(false)}
                onBuyButtonClicked={() => setModalOpen(false)}
            />
        </>
    );
}

export default ItemPage;