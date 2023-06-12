import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddLeadRequest } from './add-lead-request.model';
import { Lead } from './lead.model';
import { LeadsRequest } from './leads-request.model';
import { createQueryParams } from '../../utils/http-params-utils';
import { UpdateLeadRequest } from './update-lead-request.model';

@Injectable({
  providedIn: 'root',
})
export class LeadService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getLeads(request: LeadsRequest): Observable<Lead[]> {
    const params = createQueryParams(request);

    return this.httpClient.get<Lead[]>(`${this.environment.apiUrl}/leads`, { params });
  }

  getLead(leadName: string): Observable<Lead> {
    return this.httpClient.get<Lead>(`${this.environment.apiUrl}/leads/${leadName}`);
  }

  addLead(request: AddLeadRequest): Observable<number> {
    return this.httpClient.post<number>(`${this.environment.apiUrl}/leads`, request);
  }

  updateLead(leadName: string, request: UpdateLeadRequest): Observable<void> {
    return this.httpClient.put<void>(`${this.environment.apiUrl}/leads/${leadName}`, request);
  }

  deleteLead(leadName: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.environment.apiUrl}/leads/${leadName}`);
  }
}
