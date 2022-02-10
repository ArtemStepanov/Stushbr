import {Modal} from "react-bulma-components";
import React from "react";
import {ClientInfoRequest} from "../models/ClientInfoRequest";
import ClientInfoForm from "./ClientInfoForm";
import {Item} from "../models/Item";
import {toast} from "react-toastify";
import {StushbrApi} from "../common/StushbrApi";

function ClientInfoInputModal(props: {
    isOpen: boolean,
    itemInfo: Item | null,
    onModalClose: () => void,
    onBuyButtonClicked: () => void
}) {
    const {isOpen, itemInfo, onModalClose, onBuyButtonClicked} = props

    const handleFormSubmit = async (clientInfo: ClientInfoRequest) => {
        if (!itemInfo) {
            toast.warn("Перед совершением оплаты выберите продукт");
            return
        }

        const resp = await StushbrApi.makePayment({
            id: itemInfo.id,
            clientInfo: clientInfo
        })

        if (!resp?.url) {
            return
        }

        window.open(resp.url, "_blank")
        onBuyButtonClicked()
    }

    return (
        <Modal
            show={isOpen}
            onClose={onModalClose}
            showClose={false}
        >
            <Modal.Card p={2}>
                <Modal.Card.Header showClose>
                    <Modal.Card.Title>Расскажите о себе</Modal.Card.Title>
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <ClientInfoForm {...props} onFormSubmit={handleFormSubmit} />
                </Modal.Card.Body>
            </Modal.Card>
        </Modal>
    );
}

export default ClientInfoInputModal;