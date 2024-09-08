import { Component, OnInit } from '@angular/core';
import { Observable, of, retry, timeout } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { MakeRequestForUserPermission, UpdateUserPermissionRequest, UserInfoForCombo, UserPermissionBasedOnUser } from './models/userpermission';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ComboInfo } from '../shared/models/models';
import { CommonHttpService } from '../services/common-http.service';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { UserPermissionHttpService } from './services/user-permission-http.service';
import { errorNotification, infoNotification, successNotification } from '../shared/notifications/notification';

@Component({
  selector: 'app-user-permissions',
  templateUrl: './user-permissions.component.html',
  styleUrls: ['./user-permissions.component.scss']
})
export class UserPermissionsComponent implements OnInit {
  userPermissions$: Observable<Root<UserPermissionBasedOnUser[]>> = of();
  users$: Observable<Root<UserInfoForCombo[]>> = of();
  form: FormGroup = new FormGroup({});
  permissionForm: FormGroup = new FormGroup({});
  userSelected = false;
  designationtype$: Observable<ComboInfo[]> = of([]);

  constructor(private fb: FormBuilder, private commonHttpService: CommonHttpService, private userPermissionHttpService: UserPermissionHttpService) { }

  ngOnInit(): void {
    this.intializeForm();
    this.loadAllUsers();
    this.loadDesignation();
  }

  intializeForm() {
    this.form = this.fb.group({
      userId: ['', [Validators.required]]
    })

    this.form.controls['userId'].valueChanges.subscribe({
      next: response => {
        this.loadUserPermissionData(response);
      }
    })
  }

  loadDesignation() {
		const query =
			"SELECT TypeID as _Id, TypeName as _Value FROM tblDesignationtype";
		this.designationtype$ = this.commonHttpService
			.getComboBoxData(query).pipe(
        timeout(5000),
        retry(3)
      );
	}

  changeStatusAll(checkBox: any) {
    let value = '0';

    if (checkBox.target.checked) {
      value = '1';
    }

    if (!checkBox.target.checked) {
      value = '0';
    }

    const requestData: MakeRequestForUserPermission = {
      authDto: GetAuthDetails(),
      userId: this.form.controls['userId'].value,
      hasPermission: value
    };

    this.userPermissionHttpService.updateAllUserDesignationPermission(requestData).subscribe({
      next: response => {
        if (response.isSuccess) {
          if (value == "1") {
            successNotification("Gave full access to designation")
          } else {
            infoNotification("Revoked full access from designation")
          }
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  changeStatus(event: any, location: string, checkBox: any) {
    let value = '0';

    if (checkBox.target.checked) {
      value = '1';
    }

    if (!checkBox.target.checked) {
      value = '0';
    }

    const data: UpdateUserPermissionRequest = {
      accessLocation: location,
      authDto: GetAuthDetails(),
      event: event.value,
      hasPermission: value,
      userId: this.form.controls['userId'].value
    }

    this.userPermissionHttpService.updateUserPermissionDesignation(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message)
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  loadUserPermissionData(userId: string) {
    this.userPermissions$ = this.userPermissionHttpService.getAllUserPermissionsBasedOnUserId(userId).pipe(
      timeout(7000),
      retry(3)
    );
    this.userSelected = true;
  }

  loadAllUsers() {
    this.users$ = this.userPermissionHttpService.getAllUsers().pipe(
      timeout(7000),
      retry(3)
    );
  }
}
