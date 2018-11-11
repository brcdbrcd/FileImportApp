import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ViewFileComponent } from './view-file/view-file.component';
import { ImportFileComponent } from './import-file/import-file.component';

const routes: Routes = [
  { path: 'view/:session', component: ViewFileComponent },
  { path: 'import', component: ImportFileComponent },
  { path: '', redirectTo: '/import', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
  

 }
