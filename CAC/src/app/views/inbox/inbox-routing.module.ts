import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlertsComponent } from './alerts/alerts.component';
import { InboxComponent } from './inbox/inbox.component';
import { OpenInboxComponent } from './open-inbox/open-inbox.component';

const routes: Routes = [{
  path:'',
  component:AlertsComponent
},
{
  path:'mensajes',
  component:InboxComponent
},
{
  path:'leer',
  component:OpenInboxComponent
}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InboxRoutingModule { }
