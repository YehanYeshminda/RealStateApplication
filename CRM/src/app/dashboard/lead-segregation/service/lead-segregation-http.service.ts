import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { environment } from 'src/environments/environment.development';
import { LeadSegregationRequest, LeadStaffResponse } from '../models/lead-segragetion';
import { Root } from 'src/app/shared/models/baseResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LeadSegregationHttpService {
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
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<LeadStaffResponse[]>>(this.baseUrl + "leadAssign/GetLeadDependingOnStaff?staffId=" + staffId, auth)
  }
}
