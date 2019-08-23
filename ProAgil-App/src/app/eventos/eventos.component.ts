import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
import { ThrowStmt } from '@angular/compiler';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[] = [];
  eventos: Evento[] = [];
  evento: Evento;
  imagemMargem = 2;
  imagemLargura = 50;
  mostrarImagem = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;

  // tslint:disable-next-line: variable-name
  private _filtroLista: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService
  ) {
    this.localeService.use('pt-br');
  }

  get filtroLista(): string {
    return this._filtroLista;
  }

  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  editarEventoOpenModal(template: any, evento: Evento) {
    this.evento = evento;
    this.openModal(template);
    this.registerForm.patchValue(this.evento);
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      this.evento = Object.assign(
        { Id: this.evento.Id },
        this.registerForm.value
      );

      if (this.evento.Id) {
        this.eventoService.putEventos(this.evento).subscribe(
          (novoEvento: Evento) => {
            template.hide();
            this.getEventos();
          },
          error => {
            console.log(error);
          }
        );
      } else {
        this.eventoService.postEventos(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
          },
          error => {
            console.log(error);
          }
        );
      }
    }
  }

  filtrarEventos(filtro: string): Evento[] {
    filtro = filtro.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.Tema.toLocaleLowerCase().indexOf(filtro) !== -1
    );
  }

  ngOnInit() {
    this.getEventos();
    this.validation();
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {
    this.registerForm = this.fb.group({
      Tema: [
        '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(50)]
      ],
      Local: ['', Validators.required],
      DataEvento: ['', Validators.required],
      QtdePessoas: [
        '',
        [Validators.required, Validators.min(1), Validators.max(120)]
      ],
      ImagemUrl: ['', Validators.required],
      Telefone: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]]
    });
  }

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = eventos;
        console.log(eventos);
      },
      error => {
        console.log(error);
      }
    );
  }
}
