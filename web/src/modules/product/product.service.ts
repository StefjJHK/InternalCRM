import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from './product.model';
import { Environment, ENVIRONMENT } from '../environment';
import { AddProductRequest } from './add-product-request.model';
import { ProductsRequest } from './products-request.model';
import { createQueryParams } from '../../utils/http-params-utils';
import { UpdateProductRequest } from './update-product-request.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getProducts(request: ProductsRequest): Observable<Product[]> {
    const params = createQueryParams(request);

    return this.httpClient.get<Product[]>(`${this.environment.apiUrl}/products`, { params });
  }

  getProduct(productId: string): Observable<Product> {
    return this.httpClient.get<Product>(`${this.environment.apiUrl}/products/${productId}`);
  }

  addProduct(request: AddProductRequest): Observable<string> {
    const formData = new FormData();
    formData.append('name', request.name);
    formData.append('icon', request.icon);
    formData.append('ilProject', request.ilProject);

    return this.httpClient.post<string>(`${this.environment.apiUrl}/products`, formData);
  }

  updateProduct(productName: string, request: UpdateProductRequest): Observable<void> {
    const formData = new FormData();

    if (request.name) {
      formData.append('name', request.name);
    }

    if (request.icon) {
      formData.append('icon', request.icon);
    }

    if (request.ilProject) {
      formData.append('ilProject', request.ilProject);
    }

    return this.httpClient.put<void>(`${this.environment.apiUrl}/products/${productName}`, formData);
  }

  deleteProduct(productName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.environment.apiUrl}/products/${productName}`);
  }
}
