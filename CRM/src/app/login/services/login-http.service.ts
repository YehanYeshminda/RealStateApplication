import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Company } from '../models/company';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { Branch } from '../models/branch';
import { LoginUserRequest, LoginUserResponse } from '../models/user';
import { Root } from 'src/app/shared/models/baseResponse';

@Injectable({
  providedIn: 'root'
})
export class LoginHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  loginUser(user: LoginUserRequest): Observable<Root<LoginUserResponse>> {
    return this.http.post<Root<LoginUserResponse>>(this.baseUrl + "auth/Login", user)
  }

  getAllCompanies(): Observable<Company> {
    return this.http.get<Company>(this.baseUrl + "company/GetCompanyAll")
  }

  getAllBranches(): Observable<Branch[]> {
    return this.http.get<Branch[]>(this.baseUrl + "branchs")
  }
}
