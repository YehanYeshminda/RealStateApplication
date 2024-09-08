import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { environment } from 'src/environments/environment.development';
import { LeadViewReponse, LeadsRequest, LeadsResponse } from '../models/leads';
import { Root } from 'src/app/shared/models/baseResponse';

@Injectable({
  providedIn: 'root'
})
export class LeadsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllComapaignNosComboData(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "CampainH/GetCompaignNameId", auth)
  }

  addNewLead(data: LeadsRequest): Observable<Root<LeadsResponse>> {
    return this.http.post<Root<LeadsResponse>>(this.baseUrl + "auth/insert", data)
  }

  editExistingLead(data: LeadsRequest): Observable<Root<LeadsResponse>> {
    return this.http.post<Root<LeadsResponse>>(this.baseUrl + "auth/update", data)
  }

  getAllLeads(): Observable<Root<LeadViewReponse[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<LeadViewReponse[]>>(this.baseUrl + "auth/GetAllLeads", auth);
  }
}
