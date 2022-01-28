import {Item} from "../models/Item";
import {Stushbr} from "./Stushbr";
import {ErrorsResponse} from "../models/ErrorsResponse";
import {OrderItemResponse} from "../models/OrderItemResponse";
import {OrderItemRequest} from "../models/OrderItemRequest";

export class Api {

    public static getAvailableItems(): Promise<Item[] | null> {
        return this.fetchWithHandling<Item[]>("items/available");
    }

    public static makePayment(orderItemRequest: OrderItemRequest): Promise<OrderItemResponse | null> {
        return this.fetchWithHandling<OrderItemResponse>(
            `items/${orderItemRequest.itemId}/order`,
            {
                method: "POST",
                body: JSON.stringify(orderItemRequest.clientInfo),
                headers: {"Content-Type": "application/json"}
            }
        )
    }

    private static async fetchWithHandling<T>(url: string, options?: RequestInit | undefined): Promise<T | null> {
        const response = await fetch(url, options || {})
        if (response.status >= 200 && response.status <= 299) {
            return await response.json()
        } else {
            const error: ErrorsResponse = await response.json()
            Stushbr.showErrorPopupFromErrorResponse(error)
            return null;
        }
    }
}