import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from './customer.model';
import { AddCustomerRequest } from './add-customer.model';
import { CustomersRequest } from './customers-request.model';
import { createQueryParams } from '../../utils/http-params-utils';
import { UpdateCustomerRequest } from './update-customer.model';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getCustomers(request: CustomersRequest): Observable<Customer[]> {
    const params = createQueryParams(request);

    return this.httpClient.get<Customer[]>(`${this.environment.apiUrl}/customers`, { params });
  }

  getCustomer(customerName: string): Observable<Customer> {
    return this.httpClient.get<Customer>(`${this.environment.apiUrl}/customers/${customerName}`);
  }

  addCustomer(request: AddCustomerRequest): Observable<string> {
    return this.httpClient.post<string>(`${this.environment.apiUrl}/customers`, request);
  }

  updateCustomer(customerName: string, request: UpdateCustomerRequest): Observable<void> {
    return this.httpClient.put<void>(`${this.environment.apiUrl}/customers/${customerName}`, request);
  }

  deleteCustomer(customerName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.environment.apiUrl}/customers/${customerName}`);
  }
}
