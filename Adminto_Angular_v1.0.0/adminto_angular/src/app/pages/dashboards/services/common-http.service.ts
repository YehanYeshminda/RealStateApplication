import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { CheckForPermission, CheckForPermissionReponse, ComboInfo, DynamicExpenseTable, DynamicTable, GetDynamicexpense, SendDynamicexpense, VExpenseAccountData } from '../shared/models/models';
import { GetDynamicFormRequest, SendDynamicFormRequest } from '../staff/models/staff';

export interface LastValue {
  lastValue: string
}

@Injectable({
  providedIn: 'root'
})
export class CommonHttpService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getGetLastValueFromValue(prefix: string, tableName: string, columnName: string): Observable<LastValue> {
    return this.http.get<LastValue>(
      this.baseUrl + `Common/LeadLastValueWithPrefix?prefix=${prefix}&tableName=${tableName}&columnName=${columnName}`
    );
  }

  getComboBoxData(query: string): Observable<any> {
    return this.http.get<Observable<any>>(
      this.baseUrl + 'Common/comboData?query=' + query
    );
  }

  getComboBoxstring(query: string): Observable<any> {
		return this.http.get<Observable<any>>(
			this.baseUrl + 'common/comboDataString?query=' + query
		);
	}

  getBankNameAndIds(): Observable<Root<VExpenseAccountData[]>> {
    const authDto = GetAuthDetails();
    return this.http.post<Root<VExpenseAccountData[]>>(this.baseUrl + "PaymentSchedule/ComboVExpenseData", authDto);
  }

  addDynamicexpense(data: SendDynamicexpense): Observable<GetDynamicexpense> {
    return this.http.post<GetDynamicexpense>(this.baseUrl + "Expenses/AddItemAll", data)
  }

  getdynamicexpense(table: string): Observable<DynamicExpenseTable[]> {
    return this.http.get<DynamicExpenseTable[]>(this.baseUrl + 'Expenses/' + table);
  }

  checkForAccess(data: CheckForPermission): Observable<CheckForPermissionReponse> {
    return this.http.post<CheckForPermissionReponse>(this.baseUrl + "up/GetUserPermissionForView", data)
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
