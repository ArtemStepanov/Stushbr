import {Modal} from "react-bulma-components";
import React from "react";
import {ClientInfoRequest} from "../models/ClientInfoRequest";
import ClientInfoForm from "./ClientInfoForm";
import {Item} from "../models/Item";

function ClientInfoInputModal(props: {
    isOpen: boolean,
    itemInfo: Item | undefined,
    onModalClose: () => void,
    onBuyButtonClicked: (clientInfo: ClientInfoRequest) => void
}) {
    return (
        <Modal
            show={props.isOpen}
            onClose={props.onModalClose}
            showClose={false}
        >
            <Modal.Card p={2}>
                <Modal.Card.Header showClose>
                    <Modal.Card.Title>Расскажите о себе</Modal.Card.Title>
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <ClientInfoForm {...props} onFormSubmit={props.onBuyButtonClicked} />
                </Modal.Card.Body>
            </Modal.Card>
        </Modal>
    );
}

export default ClientInfoInputModal;