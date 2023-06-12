import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductCustomer } from './product-customer.model';

@Injectable({
  providedIn: 'root',
})
export class ProductCustomersService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getProductCustomers(productName: string): Observable<ProductCustomer[]> {
    return this.httpClient.get<ProductCustomer[]>(`${this.environment.apiUrl}/products/${productName}/customers`);
  }
}
