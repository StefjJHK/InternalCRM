import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Environment, ENVIRONMENT } from '../environment';
import { ProductStatistics } from './product-statistics.model';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getProductStatistics(): Observable<ProductStatistics[]> {
    return this.httpClient.get<ProductStatistics[]>(`${this.environment.apiUrl}/statistics/products`);
  }
}
