import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddInvoiceRequest } from './add-invoice-request.model';
import { Invoice } from './invoice.model';
import { InvoicesRequest } from './invoices-request.model';
import { createQueryParams } from '../../utils/http-params-utils';
import { UpdateInvoiceRequest } from './update-invoice-request.model';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getInvoices(request: InvoicesRequest): Observable<Invoice[]> {
    const params = createQueryParams(request);

    return this.httpClient.get<Invoice[]>(`${this.environment.apiUrl}/invoices`, { params });
  }

  getInvoice(invoiceNumber: string): Observable<Invoice> {
    return this.httpClient.get<Invoice>(`${this.environment.apiUrl}/invoices/${invoiceNumber}`);
  }

  addInvoice(request: AddInvoiceRequest): Observable<number> {
    return this.httpClient.post<number>(`${this.environment.apiUrl}/invoices`, request);
  }

  updateInvoice(invoiceNumber: string, request: UpdateInvoiceRequest): Observable<void> {
    return this.httpClient.put<void>(`${this.environment.apiUrl}/invoices/${invoiceNumber}`, request);
  }

  deleteInvoice(invoiceNumber: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.environment.apiUrl}/invoices/${invoiceNumber}`);
  }
}
