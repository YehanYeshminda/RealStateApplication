import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/baseResponse';
import { GetLeadNameAndPrefered, LeadForwardResponse, LeadForwardResponseToAdd, LeadForwardViewResponse, LeadLogResponse, LeadRequest } from '../models/leadforward';
import { environment } from 'src/environments/environment.development';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfoBank } from 'src/app/shared/models/comboInfo';

@Injectable({
  providedIn: 'root'
})
export class LeadsforwardService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllLeadForwardList(): Observable<Root<LeadForwardViewResponse[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<LeadForwardViewResponse[]>>(this.baseUrl + "leadforward/GetAllLeadForwards", auth)
  }

  getLeadNoAndIdComboData(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "auth/GetLeadNameId", auth)
  }

  getLeadNoAndIdComboDataAll(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "auth/GetLeadNameIdAll", auth)
  }

  getLeadForwardById(leadNo: string): Observable<Root<LeadLogResponse>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<LeadLogResponse>>(this.baseUrl + "leadforward/GetLeadWithLeadForward?leadNo=" + leadNo, auth);
  }

  getStaffComoboId(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "staff/GetStaffNameId", auth);
  }

  addNewLeadForward(data: LeadRequest): Observable<Root<LeadForwardResponseToAdd>> {
    return this.http.post<Root<LeadForwardResponseToAdd>>(this.baseUrl + "leadforward/AddNewLeadForward", data)
  }

  editExistingLeadForward(data: LeadRequest): Observable<Root<LeadForwardResponseToAdd>> {
    return this.http.post<Root<LeadForwardResponseToAdd>>(this.baseUrl + "leadforward/EditExistingLeadForward", data)
  }

  loadLeadNoHistoryByLeadId(leadNo: string): Observable<Root<GetLeadNameAndPrefered>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<GetLeadNameAndPrefered>>(this.baseUrl + "auth/GetLeadNameConMethod?leadNo=" + leadNo, auth)
  }
}
