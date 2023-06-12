import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddSubscriptionRequest } from './add-subscription-request.model';
import { Subscription } from './subscritpion.model';
import { SubscriptionsRequest } from './subscriptions-request.model';
import { createQueryParams } from '../../utils/http-params-utils';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  addSubscription(invoiceNumber: number, request: AddSubscriptionRequest): Observable<number> {
    return this.httpClient.post<number>(`${this.environment.apiUrl}/invoices/${invoiceNumber}/subscriptions`, request);
  }

  getSubscriptions(request: SubscriptionsRequest): Observable<Subscription> {
    const params = createQueryParams(request);

    console.log(params);

    return this.httpClient.get<Subscription>(`${this.environment.apiUrl}/subscriptions`, { params });
  }
}
