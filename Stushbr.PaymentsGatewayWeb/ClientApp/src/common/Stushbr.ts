import {ErrorsResponse} from "../models/ErrorsResponse";

export class Stushbr {

    static showErrorPopup(errorMessage: ErrorsResponse) {
        alert(`Произошла ошибка:
- ${errorMessage.errors.map(value => {
            return `${value.prop && `${value.prop}: ${value.msg}` || value.msg}`;
        }).join('\n')}`)
    }
}