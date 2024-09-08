import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { ArchivedLeadListDtoRequest, ArchivedLeadsAll } from '../models/archivedlead';

@Injectable({
  providedIn: 'root'
})
export class ArchivedLeadHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllArchivedLeads(data: ArchivedLeadListDtoRequest): Observable<Root<ArchivedLeadsAll>> {
    return this.http.post<Root<ArchivedLeadsAll>>(this.baseUrl + "archivedLeads/GetAllArchivedLeads", data)
  }
}
