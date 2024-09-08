import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { environment } from 'src/environments/environment';
import { DeleteLeadRequest, LeadFilterRequest, LeadsRequest, LeadsResponse, LeadsViewData, LogRequest, UpdateLeadStatusRequest, leadloglist, leadlogrequest } from '../models/list';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { ComboInfoBank } from '../../shared/models/models';

@Injectable({
  providedIn: 'root'
})
export class LeadsHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllLeads(page: number = 1, pageSize: number = 5): Observable<Root<LeadsViewData>> {
    const auth: any = GetAuthDetails();
    return this.http.post<Root<LeadsViewData>>(this.baseUrl + "auth/GetAllLeads", auth, {
      params: {
        page: page,
        pageSize: pageSize
      }
    })
  }

  getAllLeadsFilteredByStaff(staff: number): Observable<Root<LeadsViewData>> {
    const auth: any = GetAuthDetails();
    return this.http.post<Root<LeadsViewData>>(this.baseUrl + "auth/GetAllLeadsByStaffId", auth, {
      params: {
        staffId: staff
      }
    })
  }

  getAllLeadLogs(data: leadlogrequest): Observable<Root<leadloglist>> {
    return this.http.post<Root<leadloglist>>(this.baseUrl + "LeadLog/GetLeadLogs",data)
  }

  addNewlog(data: LogRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "LeadLog/insert", data)
  }

  addNewLead(data: LeadsRequest): Observable<Root<LeadsResponse>> {
    return this.http.post<Root<LeadsResponse>>(this.baseUrl + "auth/insert", data)
  }

  editExistingLead(data: LeadsRequest): Observable<Root<LeadsResponse>> {
    return this.http.post<Root<LeadsResponse>>(this.baseUrl + "auth/update", data)
  }

  getAllComapaignNosComboData(): Observable<ComboInfoBank[]> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<ComboInfoBank[]>(this.baseUrl + "CampainH/GetCompaignNameId", auth)
  }

  updateLeadstatus(data: UpdateLeadStatusRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "auth/UpdateLeadstatus", data);
  }

  filterLeadsStatus(data: LeadFilterRequest): Observable<Root<LeadsViewData>> {
    return this.http.post<Root<LeadsViewData>>(this.baseUrl + "auth/FilterLeadByStatus", data);
  }

  deleteLead(data: DeleteLeadRequest): Observable<Root<string>> {
    return this.http.post<Root<string>>(this.baseUrl + "auth/DeleteLeads" ,data)
  }

  filterLeadStatusAndStaff(staffId: number, importance: string, page: number = 1, pageSize: number = 10) {
    return this.http.get<Root<LeadsViewData>>(this.baseUrl + "auth/GetAllLeadsByStatusImportance", {
      params: {
        staffId: staffId,
        importance: importance,
        page: page,
        pageSize: pageSize
      }
    })
  }
}
