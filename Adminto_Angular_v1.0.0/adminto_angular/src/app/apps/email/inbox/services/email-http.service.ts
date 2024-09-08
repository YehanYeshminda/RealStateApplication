import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { environment } from 'src/environments/environment';
import { OutlookEmails } from '../email.model';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';

@Injectable({
  providedIn: 'root'
})
export class EmailHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllEmails(pages: number = 1, pageSize = 10): Observable<Root<OutlookEmails[]>> {
    const auth: AuthDetails = GetAuthDetails();
    return this.http.post<Root<OutlookEmails[]>>(this.baseUrl + "email/getemails", auth, {
      params: {
        page: pages,
        pageSize: pageSize
      }
    })
  }
}
