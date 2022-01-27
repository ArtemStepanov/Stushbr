import {ClientInfoRequest} from "./ClientInfoRequest";

export interface OrderItemRequest {
    itemId: string
    clientInfo: ClientInfoRequest
}