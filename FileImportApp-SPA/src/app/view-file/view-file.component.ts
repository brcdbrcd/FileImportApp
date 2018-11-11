import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { StoreItem } from './StoreItem';
import { StoreService } from './Store.service';
import {merge, Observable, of as observableOf} from 'rxjs';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-file',
  templateUrl: './view-file.component.html',
  styleUrls: ['./view-file.component.css']
})
export class ViewFileComponent implements OnInit {
  displayedColumns: string[] = ['Key', 'ArtikelCode', 'ColorCode',
  'Description', 'Price', 'DiscountPrice', 'DeliveredIn', 'Q1', 'Size', 'Color'];
  data: StoreItem[] = [];

  resultsLength = 0;
  isLoadingResults = true;
  session: String;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private service: StoreService,private route: ActivatedRoute) {}

  ngOnInit() {
     this.route.params.subscribe(params => {
      this.session = params['session'];
    });
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.service.getData(this.session, this.paginator.pageIndex);
        }),
        map(resp => {
          this.isLoadingResults = false;
          this.resultsLength = resp.totalCount;
          return resp.data;
        }),
        catchError(() => {
          this.isLoadingResults = false;
          return observableOf([]);
        })
      ).subscribe(data => this.data = data);
  }


}
