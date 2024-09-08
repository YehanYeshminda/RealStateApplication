import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { DesignationAll, DesignationViewData } from '../models/designation';
import { HtmlResponse } from '../../staff/models/staff';

@Injectable({
  providedIn: 'root'
})
export class DesignationHttpService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getdesignation(page: number = 1, pageSize: number = 5): Observable<Root<DesignationViewData>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<DesignationViewData>>(this.baseUrl + "designation/Get", auth, {
      params: {
        page: page,
        pageSize: pageSize
      }
    });
	}


	adddesignation(designation: DesignationAll) {
		return this.http.post<Root<DesignationAll[]>>(
			this.baseUrl + 'designation/insertdesignation',
			designation
		);
	}

	// generateReports(): Observable<HtmlResponse> {
	// 	return this.http.get<HtmlResponse>(this.baseUrl + 'designation/report');
	// }

	deletedesignation(id: number): Observable<Root<String>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<string>>(this.baseUrl + "designation/DeleteDesignation?id=" + id, auth);
	}
}
