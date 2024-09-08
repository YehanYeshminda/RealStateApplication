import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonHttpService } from '../../services/common-http.service';
import { Observable, Subscription, catchError, debounceTime, map, of, retry, shareReplay, tap, timeout } from 'rxjs';
import { ComboInfo, ComboInfoBank } from '../../shared/models/models';
import { LeadsHttpService } from '../service/leads-http.service';
import { LeadforwardHttpService } from '../../lead-forward/services/leadforward-http.service';
import { errorNotification, successNotification } from '../../shared/notifications/notification';
import { LeadVList, LeadsRequest } from '../models/list';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { Router } from '@angular/router';
import { checkForAccess } from '../../shared/methods';
import { DEBOUNCE_1000, ERROR_MESSAGE, FIVE_THOUSAND_TIMEOUT_TIME, RETRY_COUNT } from 'src/app/shared/times';
import { UnsubscribeHelper } from 'src/app/shared/helpers';

@Component({
  selector: 'app-lead-add-edit',
  templateUrl: './lead-add-edit.component.html',
  styleUrls: ['./lead-add-edit.component.scss']
})
export class LeadAddEditComponent implements OnInit, OnDestroy {
  leadsFormControl!: FormGroup;
  submitted = false;
  isEditMode = false;
  sourceNos$: Observable<ComboInfo[]> = of([]);
  isCampaign: boolean | undefined = false;
  campaignNos$: Observable<ComboInfoBank[]> = of([]);
  PreferedContactMethod$: Observable<ComboInfo[]> = of([]);
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  leadStatus$: Observable<ComboInfo[]> = of([]);
  editLeadsInfo: LeadVList | null = null;
  isAddAllowed$: Observable<boolean> = of();

  private leadLastNoSubcrtipion: Subscription | undefined;
  private loadSourceIdSubcription: Subscription | undefined;
  private patchCampaignNoSubcription: Subscription | undefined;
  private patchSourceNoSubcription: Subscription | undefined;
  private patchStaffNoSubcription: Subscription | undefined;
  private patchLeadStatusSubcription: Subscription | undefined;
  private patchPreferedContactSubcription: Subscription | undefined;
  private leadEditSubcription: Subscription | undefined;

  get form() { return this.leadsFormControl.controls; }

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService, 
    private leadHttpService: LeadsHttpService, 
    private leadForwardHttpService: LeadforwardHttpService,
    private router: Router
    ) { }

  intializeForm(data?: LeadVList) {
    if (data != null) {
      this.isEditMode = true;

      if (data.Campaign != "") {
        this.isCampaign = true;
      } else {
        this.isCampaign = false;
      }

      this.leadsFormControl = this.fb.group({
        leadno: [data.LeadNo],
        sourceid: [data.Source, [Validators.required]],
        campainid: ['', []],
        name: [data.name, [Validators.required]],
        phone: [data.phone, [Validators.required]],
        email: [data.email, [Validators.required, Validators.email]],
        otherno: [data.otherno, [Validators.required]],
        assignedTo: [data.staffName, []],
        leadStatus: [data.leadstatus, []],
        contactMethod: [data.contactMethod, [Validators.required]],
        remark: [''],
        comment: [data.comment, []]
      })

      this.loadSourceIdSubcription = this.leadsFormControl.controls['sourceid'].valueChanges.subscribe((value) => {
        this.sourceNos$.pipe(
          timeout(FIVE_THOUSAND_TIMEOUT_TIME),

          map(response => response.find(item => item.value == value)),
          map(selectedSource => this.isCampaign = selectedSource ? selectedSource.textValue === 'Campaign' : false)
        ).subscribe();
      });
      

      if (this.isCampaign) {
        this.patchCampaignNoSubcription = this.campaignNos$.subscribe({
          next: values => {
            const existingValue = values.findIndex(x => x.value == data.Campaign);
            if (existingValue == -1)
            {
              this.leadsFormControl.patchValue({
                campainid: "",
              })

              return;
            }
            this.leadsFormControl.patchValue({
              campainid: values[existingValue].value,
            })
          }
        });
      }

      this.patchSourceNoSubcription = this.sourceNos$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.Source);
          if (existingValue == -1)
          {
            this.leadsFormControl.patchValue({
              sourceid: 6,
            })

            return;
          }
          
          this.leadsFormControl.patchValue({
            sourceid: values[existingValue].value,
          });
        }
      })

      this.patchStaffNoSubcription = this.staffNos$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.staffName);

          if (existingValue == -1)
          {
            this.leadsFormControl.patchValue({
              assignedTo: 25,
            })

            return;
          }

          this.leadsFormControl.patchValue({
            assignedTo: values[existingValue].value,
          })
        }
      })

      this.patchLeadStatusSubcription =this.leadStatus$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.leadstatus);

          if (existingValue == -1)
          {
            this.leadsFormControl.patchValue({
              leadStatus: 21,
            })

            return;
          }
          
          this.leadsFormControl.patchValue({
            leadStatus: values[existingValue].value,
          })
        }
      })

      this.patchPreferedContactSubcription = this.PreferedContactMethod$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == data.contactMethod);

          if (existingValue == -1)
          {
            this.leadsFormControl.patchValue({
              contactMethod: 1,
            })

            return;
          }

          this.leadsFormControl.patchValue({
            contactMethod: values[existingValue].value,
          })
        }
      })
    } else {
      this.isEditMode = false;
      this.router.navigateByUrl("/dashboard/lead");
      // this.leadsFormControl = this.fb.group({
      //   leadno: [''],
      //   sourceid: [1, [Validators.required]],
      //   campainid: [null, []],
      //   name: ['', [Validators.required]],
      //   phone: ['', [Validators.required, Validators.pattern(MOBILE_NO_REGEX)]],
      //   email: ['', [Validators.required, Validators.email]],
      //   otherno: ['', []],
      //   assignedTo: [null, []],
      //   leadStatus: [null, []],
      //   contactMethod: ['', [Validators.required]],
      //   remark: ['']
      // });
  
      // this.leadsFormControl.controls['sourceid'].valueChanges.subscribe({
      //   next: value => {
      //     this.sourceNos$.pipe(
      //       map(response => {
      //         const selectedSource = response.find(item => item.value == value);
      //         return selectedSource ? selectedSource.textValue === 'Campaign' : false;
      //       })
      //     ).subscribe(result => {
      //       if (result === true) {
      //         this.isCampaign = true;
      //       } else {
      //         this.isCampaign = false;
      //       }
      //     });
      //   }
      // })
    }
  }

  onSubmit() {
    this.submitted = true;
    if (this.leadsFormControl.invalid) {
      errorNotification('Please enter required fields');
      return;
    }

    if (!this.isEditMode) {
      console.log("executed")
      const data: LeadsRequest = {
        authDto: GetAuthDetails(),
        ...this.leadsFormControl.value,
        campainid: this.leadsFormControl.controls['campainid'].value
      };

      this.leadHttpService.addNewLead(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.leadsFormControl.reset();
            this.router.navigateByUrl("/dashboard/lead")

            this.getLastNo();
          } else {
            errorNotification(response.message);
          }
        }
      })
    } else {
      if (typeof(this.leadsFormControl.controls['leadStatus'].value) == 'string') {
        this.leadsFormControl.controls['leadStatus'].patchValue(21);
      };

      const data: LeadsRequest = {
        authDto: GetAuthDetails(),
        ...this.leadsFormControl.value,
        campainid: this.leadsFormControl.controls['campainid'].value,
      };

      this.leadEditSubcription = this.leadHttpService.editExistingLead(data).pipe(
        timeout(FIVE_THOUSAND_TIMEOUT_TIME),
        retry(RETRY_COUNT),
        debounceTime(DEBOUNCE_1000),
        tap(response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.router.navigateByUrl("/dashboard/lead");
          } else {
            errorNotification(response.message);
          }
        }),
        catchError(error => {
          errorNotification(ERROR_MESSAGE);
          throw error;
        })
      ).subscribe();
    }
  }

  getLastNo() {
    if (!this.isEditMode) {
      this.leadLastNoSubcrtipion = this.commonHttpService.getGetLastValueFromValue('LE', 'tblcontrol', 'LeadNo').pipe(
        timeout(FIVE_THOUSAND_TIMEOUT_TIME),
        retry(RETRY_COUNT),
        debounceTime(DEBOUNCE_1000),
        tap((response) => {
          this.leadsFormControl.patchValue({
            leadno: response.lastValue
          })
        }),
      ).subscribe();
    }
  }

  loadSources() {
    const query = "SELECT ID as _Id, Source as _Value FROM tblsource WHERE status = 0 ORDER BY ID DESC"
    this.sourceNos$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  loadCompaignNos() {
    this.campaignNos$ = this.leadHttpService.getAllComapaignNosComboData().pipe(
      shareReplay(1)
    );;
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

  loadLeadStatus() {
    const query = "SELECT id as _Id, leadstatus as _Value FROM tblLeadStatus WHERE status = 0 ORDER BY id DESC"
    this.leadStatus$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  ngOnInit(): void {
    this.editLeadsInfo = history.state;

    this.loadSources();
    this.loadCompaignNos();
    this.loadStaff();
    this.loadPreferedContactMethod();
    this.loadLeadStatus();

    if (this.editLeadsInfo?.LeadNo) {
      if (this.editLeadsInfo != null) {
        this.intializeForm(this.editLeadsInfo);
        this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Edit", "Lead");
      }
    } else {
      this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Add", "Lead");
      this.intializeForm();
    }

    this.getLastNo();
  }

  ngOnDestroy(): void {
      UnsubscribeHelper(
        this.leadLastNoSubcrtipion, 
        this.loadSourceIdSubcription,
        this.patchCampaignNoSubcription,
        this.patchSourceNoSubcription,
        this.patchStaffNoSubcription,
        this.patchLeadStatusSubcription,
        this.patchPreferedContactSubcription,
        this.leadEditSubcription
      );
  }
}
