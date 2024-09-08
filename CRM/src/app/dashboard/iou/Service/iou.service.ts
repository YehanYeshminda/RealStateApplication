import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { iou, viou } from '../iou';

@Injectable({
  providedIn: 'root'
})
export class IouService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getiou(): Observable<Root<viou[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<viou[]>>(this.baseUrl + "IOU/getIou", auth);
	}

	addiou(iou: iou) {
		return this.http.post<Root<iou[]>>(
			this.baseUrl + 'IOU/insertIou',
			iou
		);
	}

	updateiou(iou: iou) {
		return this.http.post<Root<iou[]>>(
			this.baseUrl + 'IOU/updateIou',
			iou
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'IOU/report');
	}
	  
	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'IOU/cellreport/' + id);
	}
}
