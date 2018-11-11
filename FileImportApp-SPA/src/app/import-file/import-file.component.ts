import { Component, OnInit } from '@angular/core';
import { ImportService } from './import-file.service';
import { HttpEventType, HttpResponse, HttpProgressEvent, HttpErrorResponse } from '@angular/common/http';
import { ImportState } from './ImportState';
import { ImportResponse } from './ImportResponse';

@Component({
  selector: 'app-import-file',
  templateUrl: './import-file.component.html',
  styleUrls: ['./import-file.component.css']
})
export class ImportFileComponent implements OnInit {

  fileToUpload: File = null;
  buttonDisabled: Boolean = false;
  percentage = 0;
  state: String = '';
  description: String;
  timerInterval: any;
  session: String = '';

  constructor(private service: ImportService) {
  }

  setFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    console.log('file is set.');
  }

  uploadFile() {
    console.log('upload started');
    this.buttonDisabled = true;
    this.service.uploadData(this.fileToUpload).subscribe(event => {
      if (event instanceof HttpResponse) {
        this.session = event.body.session;
        this.timerInterval = setInterval(() => {
          this.service.getStatus(this.session).subscribe(resp => {
            this.updateStatus(resp);
            if (this.checkIfCompleted(resp)) {
              clearInterval(this.timerInterval);
              this.enableButtons();
            }
          }, (err) => {
            console.log('Upload Error:', err);
            this.setFailed('Cannot get status of progress.');
          }, () => {
            console.log('Upload done');
          });
        }, 500);
      } else if ((<HttpProgressEvent>event).type === HttpEventType.UploadProgress) {
        const progress: HttpProgressEvent = <HttpProgressEvent>event;
        const percentDone: number = Math.round(100 * (progress.loaded / progress.total));
        this.updateStatus({ importState: 'Uploading', percentage: percentDone, description: '' });
        console.log(`File is ${percentDone}% loaded.`);
      }
    },
      (err) => {
        console.log('Upload Error:', err);
        this.enableButtons();
        if (err.error.description){
          this.handleHTTPError(err);
        }
      }, () => {
        console.log('Upload done');
      });

  }

  private handleHTTPError(event: HttpErrorResponse) {
    this.setFailed(event.error.description);
  }

  private setFailed(description: String) {
    this.state = 'Failed';
    this.percentage = 0;
    this.description = description;
  }

  private enableButtons() {
    this.buttonDisabled = false;
  }

  private checkIfCompleted(resp: ImportState) {
    return resp.importState === 'Completed' || resp.importState === 'Failed' ;
  }

  private updateStatus(status: ImportState) {
    console.log(status);
    this.state = status.importState;
    this.percentage = status.percentage;
    this.description = status.description;
  }

  ngOnInit() {
  }

}
