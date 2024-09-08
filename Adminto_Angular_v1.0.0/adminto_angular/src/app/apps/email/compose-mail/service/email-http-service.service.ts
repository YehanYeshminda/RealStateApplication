import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SendEmailRequest } from '../models/email';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';

@Injectable({
  providedIn: 'root'
})
export class EmailHttpServiceService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  sendEmail(data: SendEmailRequest): Observable<Root<any>> {
    return this.http.post<Root<any>>(this.baseUrl + "email/sendemail", data);
  }
}
