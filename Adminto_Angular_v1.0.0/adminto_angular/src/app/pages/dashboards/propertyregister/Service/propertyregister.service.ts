import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { getAuthDetails } from '../../shared/method';
import { ComboInfo } from '../../shared/models/models';
import { HtmlResponse } from '../../staff/models/staff';
import { PropertyRegisterView,VRegListAll,propertyregister } from '../Model/propertyregister';
@Injectable({
	providedIn: 'root'
})
export class PropertyregisterService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

  	getpropertyregister(page: number = 1, pageSize: number = 5): Observable<Root<VRegListAll>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<VRegListAll>>(this.baseUrl + "PropertyRegister/getpropreg", auth, {
			params: {
				page: page,
				pageSize: pageSize
			}
		});
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "PropertyRegister/GetStaffNameIdAll", auth)
	}

	addpropertyregister(propertyregister: FormData) {
		return this.http.post<Root<propertyregister[]>>(
			this.baseUrl + 'PropertyRegister/insertpropreg',
			propertyregister
		);
	}

	updatepropertyregister(propertyregister: FormData) {
		return this.http.post<Root<propertyregister[]>>(
			this.baseUrl + 'PropertyRegister/updatepropreg',
			propertyregister
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'PropertyRegister/report');
	}

	cellreport(id: string): Observable<any> {
		return this.http.get(this.baseUrl + 'PropertyRegister/cellreport/' + id);
	}
}







