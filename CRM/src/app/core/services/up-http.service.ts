import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { UserPermission } from '../models/userpermissions';

@Injectable({
  providedIn: 'root'
})
export class UpHttpService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }


  getUserPermission(data: UserPermission) {
    return this.http.post<number>(this.baseUrl + "UserPermission/GetUserPermissionForView", data);
  }
}
