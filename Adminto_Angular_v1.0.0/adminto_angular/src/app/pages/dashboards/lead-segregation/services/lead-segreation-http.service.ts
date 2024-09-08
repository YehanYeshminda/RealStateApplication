import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LeadSegregationRequest, LeadStaffResponse } from '../models/leadsegregation';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';

@Injectable({
  providedIn: 'root'
})
export class LeadSegreationHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addNewSegregation(data: LeadSegregationRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "leadAssign/insertLeadAssign", data)
  }

  removeExistingLeadSegregation(data: LeadSegregationRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "leadAssign/removeLeadAssign", data)
  }

  removeExistingLeadSegregationSingle(data: LeadStaffResponse): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "leadAssign/removeLeadAssignSingle", data)
  }

  getAllLeadsByStaffId(staffId: number): Observable<Root<LeadStaffResponse[]>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<LeadStaffResponse[]>>(this.baseUrl + "leadAssign/GetLeadDependingOnStaff?staffId=" + staffId, auth)
  }
}
