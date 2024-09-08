import { Component, OnInit } from '@angular/core';
import { Root } from 'src/app/shared/models/baseResponse';
import { Event, MakeRequestForUserPermission, UpdateUserPermissionRequest, UserInfoForCombo, UserPermissionBasedOnUser } from './models/userpermission';
import { Observable, of } from 'rxjs';
import { UserPermissionHttpService } from './services/user-permission-http.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, infoNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { CommonHttpService } from '../common/common-http.service';

@Component({
  selector: 'app-user-permission',
  templateUrl: './user-permission.component.html',
  styleUrls: ['./user-permission.component.scss']
})
export class UserPermissionComponent implements OnInit {
  userPermissions$: Observable<Root<UserPermissionBasedOnUser[]>> = of();
  users$: Observable<Root<UserInfoForCombo[]>> = of();
  form: FormGroup = new FormGroup({});
  permissionForm: FormGroup = new FormGroup({});
  userSelected = false;
  designationtype$: Observable<ComboInfo[]> = of([]);

  constructor(private userPermissionHttpService: UserPermissionHttpService, private fb: FormBuilder, private commonHttpService: CommonHttpService) { }

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
			.getComboBoxData(query);
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
      authDto: getAuthDetails(),
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

  changeStatus(event: Event, location: string, checkBox: any) {
    let value = '0';

    if (checkBox.target.checked) {
      value = '1';
    }

    if (!checkBox.target.checked) {
      value = '0';
    }

    const data: UpdateUserPermissionRequest = {
      accessLocation: location,
      authDto: getAuthDetails(),
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
    this.userPermissions$ = this.userPermissionHttpService.getAllUserPermissionsBasedOnUserId(userId);
    this.userSelected = true;
  }

  loadAllUsers() {
    this.users$ = this.userPermissionHttpService.getAllUsers();
  }
}
