import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddPurchaseOrderRequest } from './add-purchase-order-request';
import { PurchaseOrder } from './purchase-order.model';
import { PurchaseOrdersRequest } from './purchase-orders-request.model';
import { createQueryParams } from '../../utils/http-params-utils';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getPurchaseOrders(request: PurchaseOrdersRequest): Observable<PurchaseOrder[]> {
    const params = createQueryParams(request);

    return this.httpClient.get<PurchaseOrder[]>(`${this.environment.apiUrl}/purchase-orders`, { params });
  }

  getPurchaseOrder(purchaseOrderNumber: string): Observable<PurchaseOrder> {
    return this.httpClient.get<PurchaseOrder>(`${this.environment.apiUrl}/purchase-orders/${purchaseOrderNumber}`);
  }

  addPurchaseOrder(request: AddPurchaseOrderRequest): Observable<number> {
    return this.httpClient.post<number>(`${this.environment.apiUrl}/purchase-orders`, request);
  }
}
