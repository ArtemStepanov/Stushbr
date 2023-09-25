import {ErrorsResponse} from "../models/ErrorsResponse";
import {toast} from "react-toastify";
import {Item} from "../models/Item";
import {OrderItemRequest} from "../models/OrderItemRequest";
import {OrderItemResponse} from "../models/OrderItemResponse";

export class StushbrApi {
    static getItemById(id: string): Promise<Item | null> {
        return Fetcher.fetchWithHandling<Item>(`items/${id}`)
    }

    static getAvailableItems(): Promise<Item[] | null> {
        return Fetcher.fetchWithHandling<Item[]>("items/available");
    }

    static makePayment(orderItemRequest: OrderItemRequest): Promise<OrderItemResponse | null> {
        return Fetcher.fetchWithHandling<OrderItemResponse>(
            "items/order",
            {
                method: "POST",
                body: JSON.stringify(orderItemRequest),
                headers: {"Content-Type": "application/json"}
            }
        )
    }
}

class Fetcher {
    static async fetchWithHandling<T>(url: string, options?: RequestInit | undefined): Promise<T | null> {
        const response = await fetch(url, options || {})
        if (response.status >= 200 && response.status <= 299) {
            return await response.json()
        } else {
            const error: ErrorsResponse = await response.json()
            this.showErrorPopupFromErrorResponse(error)
            return null;
        }
    }

    private static showErrorPopupFromErrorResponse(errorMessage: ErrorsResponse) {
        const message = `Произошла ошибка: ${errorMessage.errors.map(value => value.msg).join("; ")}`
        toast.warn(message, {position: "top-right", autoClose: 3000});
    }
}
