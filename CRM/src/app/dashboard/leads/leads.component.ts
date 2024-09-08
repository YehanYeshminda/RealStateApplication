import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, map, of, shareReplay } from 'rxjs';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { LeadsService } from './Service/leads.service';
import { CommonHttpService } from '../common/common-http.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LeadViewReponse, LeadsRequest } from './models/leads';
import { checkForAccess, getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { TypeComponent } from '../type/type.component';
import { LeadsforwardService } from '../leadsforward/Service/leadsforward.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-leads',
  templateUrl: './leads.component.html',
  styleUrls: ['./leads.component.scss']
})
export class LeadsComponent implements OnInit, OnDestroy {
  campaignNos$: Observable<ComboInfoBank[]> = of([]);
  sourceNos$: Observable<ComboInfo[]> = of([]);
  leadStatus$: Observable<ComboInfo[]> = of([]);
  PreferedContactMethod$: Observable<ComboInfo[]> = of([]);
  form: FormGroup = new FormGroup({});
  isEditMode: boolean = false;
  editLeadsInfo: LeadViewReponse | null = null;
  modalRef?: BsModalRef;
  isCampaign: boolean | undefined = false;
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  isAddAllowed$: Observable<boolean> = of();

  constructor(
    private leadsHttpService: LeadsService,
    private commonHttpService: CommonHttpService,
    private fb: FormBuilder,
    private modalService: BsModalService,
    private leadForwardHttpService: LeadsforwardService,
    private router: Router) { }

  ngOnDestroy(): void {
    this.editLeadsInfo = null;
  }

  ngOnInit(): void {
    this.editLeadsInfo = history.state;
    this.loadSources();
    this.loadCompaignNos();
    this.loadStaff();
    this.loadLeadStatus();
    this.loadPreferedContactMethod();

    if (this.editLeadsInfo?.LeadNo) {
      if (this.editLeadsInfo != null) {
        this.initializeForm(this.editLeadsInfo);
        this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Edit", "Lead");
      }
    } else {
      this.initializeForm();
      this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Add", "Lead");
    }

    this.getLastNo();
  }

  initializeForm(data?: LeadViewReponse) {
    if (data != null) {
      this.isEditMode = true;

      if (data.Campaign != "") {
        this.isCampaign = true;
      } else {
        this.isCampaign = false;
      }

      this.form = this.fb.group({
        leadno: [data.LeadNo],
        sourceid: [data.Source, [Validators.required]],
        campainid: ['', []],
        name: [data.name, [Validators.required]],
        phone: [data.phone, [Validators.required]],
        email: [data.email, [Validators.required, Validators.email]],
        otherno: [data.otherno, [Validators.required]],
        assignedTo: [data.assigned, []],
        leadStatus: [data.leadstatus, []],
        contactMethod: [data.contactMethod, [Validators.required]],
      })

      this.campaignNos$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.value == data.Campaign);
          this.form.patchValue({
            campainid: values[existingValue].value,
          })
        }
      })

      this.sourceNos$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.Source);
          this.form.patchValue({
            sourceid: values[existingValue].value,
          })
        }
      })

      this.leadStatus$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.leadstatus);
          this.form.patchValue({
            leadStatus: values[existingValue].value,
          })
        }
      })

      this.PreferedContactMethod$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.contactMethod);
          this.form.patchValue({
            contactMethod: values[existingValue].value,
          })
        }
      })


    } else {
      this.isEditMode = false;
      this.form = this.fb.group({
        leadno: [''],
        sourceid: [1, [Validators.required]],
        campainid: [null, []],
        name: ['', [Validators.required]],
        phone: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        otherno: ['', []],
        assignedTo: [null, []],
        leadStatus: [null, []],
        contactMethod: ['', [Validators.required]],
      })
    }

    this.form.controls['sourceid'].valueChanges.subscribe({
      next: value => {
        this.sourceNos$.pipe(
          map(response => {
            const selectedSource = response.find(item => item.value == value);
            return selectedSource ? selectedSource.textValue === 'Campaign' : false;
          })
        ).subscribe(result => {
          if (result === true) {
            this.isCampaign = true;
          } else {
            this.isCampaign = false;
          }
        });
      }
    })

  }

  onSubmit() {
    if (this.form.invalid) {
      errorNotification('Please enter required fields');
      return;
    }

    if (!this.isEditMode) {
      const data: LeadsRequest = {
        authDto: getAuthDetails(),
        ...this.form.value,
        campainid: this.form.controls['campainid'].value
      };

      this.leadsHttpService.addNewLead(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.form.reset();
            this.router.navigateByUrl("/dashboard/leadslist")

            this.getLastNo();
          } else {
            errorNotification(response.message);
          }
        }
      })
    } else {
      const data: LeadsRequest = {
        authDto: getAuthDetails(),
        ...this.form.value,
        campainid: this.form.controls['campainid'].value,
      };

      this.leadsHttpService.editExistingLead(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.router.navigateByUrl("/dashboard/leadslist")
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  }

  NavigatetoSource(isSource: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isSource: isSource
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  Navigatetoleadstatus(isLeadStatus: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isLeadStatus: isLeadStatus
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  NavigatetoPreffered(isPreffered: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isPreffered: isPreffered
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }


  loadCompaignNos() {
    this.campaignNos$ = this.leadsHttpService.getAllComapaignNosComboData().pipe(
      shareReplay(1)
    );;
  }

  loadSources() {
    const query = "SELECT ID as _Id, Source as _Value FROM tblsource WHERE status = 0 ORDER BY ID DESC"
    this.sourceNos$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  loadLeadStatus() {
    const query = "SELECT id as _Id, leadstatus as _Value FROM tblLeadStatus WHERE status = 0 ORDER BY id DESC"
    this.leadStatus$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    )
  }

  loadPreferedContactMethod() {
    const query = "SELECT ID as _Id, ContactMethod as _Value FROM tblPreferedContactMethod WHERE status = 0 ORDER BY ID DESC"
    this.PreferedContactMethod$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('LE', 'tblcontrol', 'LeadNo').subscribe({
        next: response => {
          this.form.patchValue({
            leadno: response.lastValue
          })
        }
      });
    }
  }
}
