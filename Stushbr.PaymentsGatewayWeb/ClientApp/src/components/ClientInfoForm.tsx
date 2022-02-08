import React, {useState} from 'react';
import {Button, Form, Icon, Notification} from "react-bulma-components";
import {ClientInfoRequest} from "../models/ClientInfoRequest";
import {faUser, faEnvelope, faExclamationTriangle, faCheck, faMobile} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {Item} from "../models/Item";
import InputMask from "react-input-mask"
import validator from "validator";

const fieldRequiredDanger = <Form.Help color="danger">Необходимо правильно заполнить это поле</Form.Help>
const warningIcon = (
    <Icon align="right" size="small">
        <FontAwesomeIcon icon={faExclamationTriangle} />
    </Icon>
)
const okIcon = (
    <Icon align="right" size="small">
        <FontAwesomeIcon icon={faCheck} />
    </Icon>
)
const userIcon = (
    <Icon align="left" size="small">
        <FontAwesomeIcon icon={faUser} />
    </Icon>
)
const emailIcon = (
    <Icon align="left" size="small">
        <FontAwesomeIcon icon={faEnvelope} />
    </Icon>
)
const phoneIcon = (
    <Icon align="left" size="small">
        <FontAwesomeIcon icon={faMobile} />
    </Icon>
)

function ClientInfoForm(props: { onFormSubmit: (clientInfo: ClientInfoRequest) => void, itemInfo: Item | undefined }) {
    const [firstName, setFirstName] = useState<string>();
    const [firstNameWrong, setFirstNameWrong] = useState<boolean>(false)

    const [secondName, setSecondName] = useState<string>();
    const [secondNameWrong, setSecondNameWrong] = useState<boolean>(false)

    const [email, setEmail] = useState<string>();
    const [emailWrong, setEmailWrong] = useState<boolean>(false)

    const [phoneNumber, setPhoneNumber] = useState<string>();
    const [phoneWrong, setPhoneWrong] = useState<boolean>(false)

    const [tocAgreed, setTocAgreed] = useState<boolean>(false);
    const [tocAgreedWrong, setTocAgreedWrong] = useState<boolean>(false)

    const resetFormErrorsState = () => {
        setFirstNameWrong(false)
        setSecondNameWrong(false)
        setEmailWrong(false)
        setPhoneWrong(false)
        setTocAgreedWrong(false)
    }

    const verifyForm = () => {
        let error = false
        resetFormErrorsState()

        if (!firstName) {
            setFirstNameWrong(true)
            error = true
        }

        if (!secondName) {
            setSecondNameWrong(true)
            error = true
        }

        if (!email || !validator.isEmail(email)) {
            setEmailWrong(true)
            error = true
        }

        if (!phoneNumber || !validator.isMobilePhone(phoneNumber, "ru-RU")) {
            setPhoneWrong(true)
            error = true
        }

        if (!tocAgreed) {
            setTocAgreedWrong(true)
            error = true
        }

        return !error
    }

    const handleSubmitClick = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault()

        if (!verifyForm()) {
            return
        }

        props.onFormSubmit({
            email: email!,
            phoneNumber: phoneNumber!,
            firstName: firstName!,
            secondName: secondName!
        })
    }

    return (
        <form onSubmit={handleSubmitClick}>
            <Form.Field>
                <Form.Label>Имя</Form.Label>
                <Form.Control>
                    <Form.Input
                        color={!firstNameWrong ? "" : "danger"}
                        value={firstName}
                        onChange={(e) => setFirstName(e.target.value)}
                        placeholder="Анастасия"
                    />
                    {userIcon}
                    {firstNameWrong ? warningIcon : okIcon}
                </Form.Control>
                {firstNameWrong && fieldRequiredDanger}
            </Form.Field>
            <Form.Field>
                <Form.Label>Фамилия</Form.Label>
                <Form.Control>
                    <Form.Input
                        color={!secondNameWrong ? "" : "danger"}
                        value={secondName}
                        onChange={(e) => setSecondName(e.target.value)}
                        placeholder="Степанова"
                    />
                    {userIcon}
                    {secondNameWrong ? warningIcon : okIcon}
                </Form.Control>
                {secondNameWrong && fieldRequiredDanger}
            </Form.Field>
            <Form.Field>
                <Form.Label>Email</Form.Label>
                <Form.Control>
                    <Form.Input
                        color={!emailWrong ? "" : "danger"}
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="my@email.com"
                    />
                    {emailIcon}
                    {emailWrong ? warningIcon : okIcon}
                </Form.Control>
                {emailWrong && <Form.Help color="danger">Необходимо ввести правильный электронный адрес</Form.Help>}
            </Form.Field>
            <Form.Field>
                <Form.Label>Номер телефона</Form.Label>
                <Form.Control>
                    <InputMask
                        mask="+7 999 999 99 99"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value.replace(/\s/g, ""))}
                    >
                        {(inputProps: any) => (
                            <Form.Input
                                {...inputProps}
                                color={!phoneWrong ? "" : "danger"}
                                placeholder="+7 999 999 99 99"
                            />
                        )}
                    </InputMask>
                    {phoneIcon}
                    {phoneWrong ? warningIcon : okIcon}
                </Form.Control>
                {phoneWrong && <Form.Help color="danger">Необходимо ввести правильный номер телефона</Form.Help>}
            </Form.Field>

            <Form.Field>
                <Form.Control>
                    <Form.Checkbox
                        checked={tocAgreed}
                        onChange={(e) => setTocAgreed(e.target.checked)}
                    >
                        {"  "}Даю согласие с <a href="https://stushbr.ru/privacy/" target="_blank" rel="noreferrer">условиями
                        обработки
                        персональных данных</a>
                    </Form.Checkbox>
                </Form.Control>
                {tocAgreedWrong && <Form.Help color="danger">Необходимо отметить галочку</Form.Help>}
            </Form.Field>

            <Notification color="warning" light textAlign="center" textSize={7}>
                При нажатии на кнопку <b>"Оплатить"</b>, для совершения платежа, вы будете перенаправлены на сайт платёжного шлюза
            </Notification>

            {props.itemInfo && (
                <Form.Field kind="group" align="center">
                    <Form.Control>
                        <Button color="link">Оплатить {props.itemInfo.price}₽</Button>
                    </Form.Control>
                </Form.Field>
            )}

        </form>
    );
}

export default ClientInfoForm;