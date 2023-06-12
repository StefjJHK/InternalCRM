import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddPaymentRequest } from './add-payment-request.modal';
import { Payment } from './payment.model';
import { PaymentsRequest } from './payments-request.model';
import { createQueryParams } from '../../utils/http-params-utils';
import { UpdatePaymentRequest } from './update-payment-request.model';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getPayments(request: PaymentsRequest): Observable<Payment[]> {
    const params = createQueryParams(request);

    return this.httpClient.get<Payment[]>(`${this.environment.apiUrl}/payments`, { params });
  }

  addPayment(invoiceNumber: string, request: AddPaymentRequest): Observable<string> {
    return this.httpClient.post<string>(`${this.environment.apiUrl}/invoices/${invoiceNumber}/payments`, request);
  }

  updatePayment(invoiceNumber: string, paymentNumber: string, request: UpdatePaymentRequest): Observable<void> {
    return this.httpClient.put<void>(`${this.environment.apiUrl}/invoices/${invoiceNumber}/payments/${paymentNumber}`, request);
  }

  deletePayment(invoiceNumber: string, paymentNumber: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.environment.apiUrl}/invoices/${invoiceNumber}/payments/${paymentNumber}`);
  }
}
