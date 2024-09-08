import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/baseResponse';
import { environment } from 'src/environments/environment.development';
import { MakeRequestForUserPermission, UpdateUserPermissionRequest, UserInfoForCombo, UserPermissionBasedOnUser } from '../models/userpermission';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { staff } from '../../staff/staff';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<Root<UserInfoForCombo[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<UserInfoForCombo[]>>(this.baseUrl + "user/GetAllUserNameId", auth)
  }

  getAllUserPermissionsBasedOnUserId(userId: string): Observable<Root<UserPermissionBasedOnUser[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<UserPermissionBasedOnUser[]>>(this.baseUrl + "user/GetAllUserComboPermission?userId=" + userId, auth);
  }

  updateUserPermission(data: UpdateUserPermissionRequest): Observable<Root<string>> {
    return this.http.put<Root<string>>(this.baseUrl + "up/UpdateUserPermission", data);
  }

  updateUserPermissionDesignation(data: UpdateUserPermissionRequest): Observable<Root<string>> {
    return this.http.put<Root<string>>(this.baseUrl + "up/UpdateUserPermissionDesignation", data);
  }

  updateAllUserPermission(data: MakeRequestForUserPermission): Observable<Root<string>> {
    return this.http.put<Root<string>>(this.baseUrl + "up/UpdateAllUserPermission", data);
  }

  updateAllUserDesignationPermission(data: MakeRequestForUserPermission): Observable<Root<string>> {
    return this.http.put<Root<string>>(this.baseUrl + "up/UpdateAllUserPermissionDesignation", data);
  }

  getAllUserPermissionsForHome(): Observable<Root<string[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<string[]>>(this.baseUrl + "up/GetToSideNavShow", auth)
  }


  getloginuser(): Observable<Root<staff[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<staff[]>>(this.baseUrl + "auth/Getloginusername", auth);
  }
  
}
