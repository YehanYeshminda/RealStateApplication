import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { BehaviorSubject, Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { propassignv, propertyassign } from '../propertyassign';
@Injectable({
	providedIn: 'root'
})
export class PropertyassignService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

	private usernameImage: BehaviorSubject<string> = new BehaviorSubject<string>(
		''
	);

	getUserImage() {
		return this.usernameImage.asObservable();
	}

	setUserImage(image: string) {
		this.usernameImage.next(image);
	}

	getpropassign(): Observable<Root<propassignv[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<propassignv[]>>(this.baseUrl + "PropertyAssign/getpropassign", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "PropertyAssign/GetStaffNameIdAll", auth)
	}

	addpropassign(propassign: propertyassign) {
		return this.http.post<Root<propertyassign[]>>(
			this.baseUrl + 'PropertyAssign/insertpropassign',
			propassign
		);
	}

	updatepropassign(propassign: propertyassign) {
		return this.http.post<Root<propertyassign[]>>(
			this.baseUrl + 'PropertyAssign/updatepropassign',
			propassign
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'PropertyAssign/report');
	}

	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'PropertyAssign/cellreport/' + id);
	}
}
