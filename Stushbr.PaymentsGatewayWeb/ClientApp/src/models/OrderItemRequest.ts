import {ClientInfoRequest} from "./ClientInfoRequest";

export interface OrderItemRequest {
    id: string
    clientInfo: ClientInfoRequest
}