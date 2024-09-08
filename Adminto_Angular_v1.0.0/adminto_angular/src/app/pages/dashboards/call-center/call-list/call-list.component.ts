import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { Observable, Subscription, of, retry, tap, timeout } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { CallListPaginationData, calllist } from './models/calllist';
import { CallListHttpService } from './services/call-list-http.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { errorNotification, infoNotification } from '../../shared/notifications/notification';

@Component({
  selector: 'app-call-list',
  templateUrl: './call-list.component.html',
  styleUrls: ['./call-list.component.scss']
})
export class CallListComponent implements OnInit, OnDestroy {
  activeWizard4: number = 1;
  calllistinfo$: Observable<Root<CallListPaginationData>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;
  selectedFile: File | null = null;
  isuploading = false;
  isShowTitles = false;
  isShowdata = false;
  loadedFiledTitles$: Observable<Root<string[]>> = of();
  loadedFiledData$: Observable<Root<string[]>> = of();
  selectedColumns: { [key: string]: boolean } = {};
  selectedColumnsheader: { [key: string]: boolean } = {};
  uploadSecond = false;
  uploadThird = false;
  selectedCRMField: string = ''; 
  selectedFileHeaders: string[] = [];
  selectedFileFirstRow: string[] = [];
  CRMColumns: string[] = [];
  selectedHeaderColumns: string[] = [];  
  selectedDataColumns: string[] = [];
  selectedFileHeadersnew: string[] = this.selectedFileHeaders.slice();
  loadCallsSubscription!: Subscription;
  

  constructor(private callListHttpService: CallListHttpService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.loadCallListInfo(this.page);   

  }

  ngOnDestroy(): void {
    if (this.loadCallsSubscription) {
      this.loadCallsSubscription.unsubscribe();
    }
  }

  countSelectedColumns(): number {
    return Object.values(this.selectedColumns).filter(selected => selected).length;
  }

  loadCallListInfo(page: number) {
    this.loadCallsSubscription = this.callListHttpService.getcalllist(page, this.pageSize).pipe(
      timeout(5000),
      retry(3),
      tap(response => {
        if (response.isSuccess) {
          this.calllistinfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }),
    ).subscribe();
  }

  toggleSelectedColumn(item: string): void {
    if (this.countSelectedColumns() >= 3 && !this.selectedColumns[item]) {
      return;
    }
  
    if (this.selectedColumns[item]) {
      delete this.selectedColumns[item]; 
    } else {
      this.selectedColumns[item] = true;
      this.selectedFileHeadersnew = Object.keys(this.selectedColumns);
      console.log(this.selectedFileHeadersnew)
    }
  }

  onColumnSelected(column: string) {
    this.selectedColumnsheader[column] = !this.selectedColumnsheader[column];

    if (this.selectedColumnsheader[column]) {
      this.selectedDataColumns.push(column);
    } else {
      const index = this.selectedDataColumns.indexOf(column);
      if (index !== -1) {
        this.selectedDataColumns.splice(index, 1);
      }
    }
  }

  clearSelection(header: string) {
    this.selectedColumnsheader[header] = false;
  }

  populateSelect() {
    this.selectedFileHeadersnew.forEach(column => {
      this.selectedColumnsheader[column] = false;
    });
  }  
  
  tableRefresh() {
    this.loadCallListInfo(1);
    this.isPaginating = true;
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadCallListInfo(newPage);
  }

  uploadContacts(content: TemplateRef<NgbModal>): void {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  onFileSelected(event: any) {
		this.selectedFile = event.target.files[0];
    this.selectedColumns = {};
  }

  isUploadComplete = false;
 
  onUpload2() {
    if (this.selectedFile) {
      this.isShowTitles = true;
      this.isShowdata = true;
      
      if (this.isShowTitles) {
        this.loadedFiledTitles$ = this.callListHttpService.loadAllCallListDataTitles(this.selectedFile);    
      }

      if (this.isShowdata) {
        this.loadedFiledData$ = this.callListHttpService.loadAllCallListData(this.selectedFile);
        this.uploadThird = true;
      }

      if (this.uploadThird) {
        this.onSubmit();
        this.isUploadComplete = true;

        if (this.activeWizard4 === 1) {
          this.activeWizard4 = 2;
        } else if (this.activeWizard4 === 2) {
          this.activeWizard4 = 3;
        }else if (this.activeWizard4 === 3) {
          this.activeWizard4 = 4;
        }
      }

      this.callListHttpService.loadAllCallListDataTitles(this.selectedFile).subscribe(
        (response) => {
          if (response.isSuccess) {            
            this.selectedFileHeaders = response.result; 
            this.uploadSecond = true;
            this.isUploadComplete = true;
  
          } else {
            errorNotification('Error loading data: ' + response.message);
          }
        },
        (error) => {
          errorNotification('Error loading data: ' + error);
        }
      );

      this.callListHttpService.loadAllCallListData(this.selectedFile).subscribe(
        (response) => {
          if (response.isSuccess) {
            this.selectedFileFirstRow = response.result;  
            this.uploadSecond = true;
            this.isUploadComplete = true;
          } else {
            errorNotification('Error loading data: ' + response.message);
          }
        },
        (error) => {
          errorNotification('Error loading data: ' + error);
        }
      );

      this.callListHttpService.getcolumns().subscribe(
        (response) => {
          if (response.isSuccess) {
            this.CRMColumns = response.result;
          } else {
            errorNotification('Error loading data: ' + response.message);
          }
        },
        (error) => {
          errorNotification('Error loading data: ' + error);
        }
      );

      this.selectedHeaderColumns = this.selectedFileHeadersnew.map(() => '');
    } else {
      infoNotification('Please select a file to upload');
    }
  }
  

  getDefaultName(header: string): string {
    return header.toUpperCase(); 
  }

  onSubmit() {
    const selectedColumnsArray = Object.entries(this.selectedColumns)
      .filter(([columnName, selected]) => selected)
      .map(([columnName]) => ({ columnName }))
  
    if (selectedColumnsArray.length > 2) {
          if (!this.selectedFile) return errorNotification("Missing File Upload.");
  
          this.isuploading = true;
          this.isPaginating = true;
          this.isShowTitles = false;
          
  
          this.callListHttpService.uploadExcelFile(this.selectedFile, selectedColumnsArray[0].columnName, selectedColumnsArray[1].columnName, selectedColumnsArray[2].columnName).subscribe(
            response => {
              if (response.isSuccess) {  
                this.loadCallListInfo(this.page);
                this.isUploadComplete = true;
                this.isuploading = false;
                this.isPaginating = false;
  
              } else {
                errorNotification('Error uploading file: ' + response.message);
              }
            },
            error => {
              errorNotification(error);
            }
          );
    }
  }

  resetModalState(): void {
    this.selectedFile = null;
    this.selectedColumns = {};
    this.selectedFileHeaders = [];
    this.selectedFileHeadersnew = [];
    this.selectedFileFirstRow = [];
    this.CRMColumns = [];
    this.selectedHeaderColumns = [];
    this.isShowTitles = false;
    this.isShowdata = false;
    this.uploadSecond = false;
    this.uploadThird = false;
    this.isUploadComplete = false;
    this.activeWizard4 = 1;
  }
  
  closethis(content: TemplateRef<NgbModal>): void {
    this.resetModalState(); 
    this.modalService.dismissAll(content);
  }
  
}
