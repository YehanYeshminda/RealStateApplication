import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { environment } from 'src/environments/environment';
import { LeadLogRequest, LeadLogViewResponse } from '../models/log';

@Injectable({
  providedIn: 'root'
})
export class LeadLogHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  leadLogResponse(data: LeadLogRequest): Observable<Root<LeadLogViewResponse[]>> {
    return this.http.post<Root<LeadLogViewResponse[]>>(this.baseUrl + "LeadLog/Get", data)
  }
}
