import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ComboInfoBank } from '../../shared/models/models';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { GetLeadNameAndPrefered, LeadForwardResponseToAdd, LeadLogResponse, LeadRequest, LeadsForwardViewData } from '../models/leadforward';
import { Root } from 'src/app/shared/models/base';

@Injectable({
  providedIn: 'root'
})
export class LeadforwardHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getStaffComoboId(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "staff/GetStaffNameId", auth);
  }

  getAllLeadForwardList(page: number = 1, pageSize: number = 5): Observable<Root<LeadsForwardViewData>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<LeadsForwardViewData>>(this.baseUrl + "leadforward/GetAllLeadForwardsNew", auth, {
      params: {
        page: page,
        pageSize: pageSize
      }
    })
  }

  getLeadNoAndIdComboData(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "auth/GetLeadNameId", auth)
  }

  getLeadNoAndIdComboDataAll(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "auth/GetLeadNameIdAll", auth)
  }

  getLeadForwardById(leadNo: string): Observable<Root<LeadLogResponse>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<LeadLogResponse>>(this.baseUrl + "leadforward/GetLeadWithLeadForward?leadNo=" + leadNo, auth);
  }

  addNewLeadForward(data: LeadRequest): Observable<Root<LeadForwardResponseToAdd>> {
    return this.http.post<Root<LeadForwardResponseToAdd>>(this.baseUrl + "leadforward/AddNewLeadForward", data)
  }

  editExistingLeadForward(data: LeadRequest): Observable<Root<LeadForwardResponseToAdd>> {
    return this.http.post<Root<LeadForwardResponseToAdd>>(this.baseUrl + "leadforward/EditExistingLeadForward", data)
  }

  loadLeadNoHistoryByLeadId(leadNo: string): Observable<Root<GetLeadNameAndPrefered>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<GetLeadNameAndPrefered>>(this.baseUrl + "auth/GetLeadNameConMethod?leadNo=" + leadNo, auth)
  }
}
