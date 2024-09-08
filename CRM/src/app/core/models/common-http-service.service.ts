import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { DynamicExpenseTable, DynamicTable } from './dynamic';
import { ComboInfo } from './combo';
import { GetDynamicFormRequest, SendDynamicFormRequest } from 'src/app/dashboard/dynamic-form/models/dynamicForm';
import { GetDynamicexpense, SendDynamicexpense } from 'src/app/dashboard/dynamic-expenseaccount/dynamicexpenses';

export interface RequestBodyCombo {
	query: string;
}

export interface GetCatergoryTypeById {
	id: number
	subCategory: string
	remark: string
	status: number
	cid?: number
	catergoryId: number
}

@Injectable({
	providedIn: 'root',
})
export class CommonHttpServiceService {
	baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) { }

	getComboBoxData(query: string): Observable<any> {
		return this.http.get<Observable<any>>(
			this.baseUrl + 'Common/comboId?query=' + query
		);
	}

	getTableCommonData(table: string): Observable<DynamicTable[]> {
		return this.http.get<DynamicTable[]>(this.baseUrl + 'Common/' + table);
	}

	getCatergorySubCatergoryById(id: number): Observable<ComboInfo[]> {
		return this.http.get<ComboInfo[]>(this.baseUrl + 'Item/GetByCatergoryId', {
			params: {
				id: id
			}
		})
	}

	
	addDynamicData(data: SendDynamicFormRequest): Observable<GetDynamicFormRequest> {
		return this.http.post<GetDynamicFormRequest>(this.baseUrl + "common/AddItemAll", data)
	}


}
