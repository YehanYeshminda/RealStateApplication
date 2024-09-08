import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { PropertyRegisterView, propertyregister } from '../propertyregister';
@Injectable({
	providedIn: 'root'
})
export class PropertyregistrationService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

	getpropertyregister(): Observable<Root<PropertyRegisterView[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<PropertyRegisterView[]>>(this.baseUrl + "PropertyRegister/getpropreg", auth);
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
