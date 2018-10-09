using PersonnelManager.Business.Exceptions;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PersonnelManager.Business.Services
{
    public class ServiceEmploye
    {
        private readonly IDataEmploye dataEmploye;

        public ServiceEmploye(IDataEmploye dataEmploye)
        {
            this.dataEmploye = dataEmploye;
        }

        public Ouvrier GetOuvrier(string nom)
        {
            return this.dataEmploye.GetListeOuvriers().FirstOrDefault(x => x.Nom == nom);
        }

        public Ouvrier GetOuvrier(int idOuvrier)
        {
            return this.dataEmploye.GetOuvrier(idOuvrier);
        }

        public Cadre GetCadre(int idCadre)
        {
            return this.dataEmploye.GetCadre(idCadre);
        }

        public void EnregistrerCadre(Cadre cadre)
        {
            if (cadre == null)
            {
                throw new InvalidOperationException();
            }

            if (cadre.DateEmbauche.Year <= 1920)
            {
                throw new BusinessException("La date d'embauche doit être > 1920");
            }

            if (cadre.DateEmbauche >= DateTime.Today.AddMonths(3))
            {
                throw new BusinessException("La date d'embauche ne doit pas être au-delà de 3 mois");
            }

            if (cadre.SalaireMensuel < 0)
            {
                throw new BusinessException("Le salaire d'un cadre ne peut pas être négatif");
            }

            Regex regex = new Regex(@"^[A-Z][a-z\D\-\'][^$@#^%§!\p{P}\*""]+$");
            Match matchNom = regex.Match(cadre.Nom);
            Match matchPrenom = regex.Match(cadre.Prenom);
            //if(regex.IsMatch(cadre.Nom))
            if (!matchNom.Success || !matchPrenom.Success)
            {
                throw new BusinessException("Le nom et le prénom ne doivent pas contenir de caractères spéciaux");
            }

            this.dataEmploye.EnregistrerCadre(cadre);
        }

        public void EnregistrerOuvrier(Ouvrier ouvrier)
        {
            if (ouvrier == null)
            {
                throw new InvalidOperationException();
            }

            if (ouvrier.TauxHoraire <= 0)
            {
                throw new BusinessException("Le taux horaire d'un ouvrier ne peut pas être négatif");
            }

            if (ouvrier.DateEmbauche.Year <= 1920)
            {
                throw new BusinessException("La date d'embauche doit être > 1920");
            }
            if (ouvrier.DateEmbauche >= DateTime.Today.AddMonths(3))
            {
                throw new BusinessException("La date d'embauche ne doit pas être au-delà de 3 mois");
            }

            this.dataEmploye.EnregistrerOuvrier(ouvrier);
        }

        public IEnumerable<Employe> GetListeEmployes()
        {
            var listeEmployes = new List<Employe>();
            listeEmployes.AddRange(this.dataEmploye.GetListeOuvriers());
            listeEmployes.AddRange(this.dataEmploye.GetListeCadres());

            return listeEmployes.OrderBy(x => x.Nom).ThenBy(x => x.Prenom);
        }

        public IEnumerable<SalaireOuvrier> GetSalaireOuvrier(int idOuvrier, DateTime mois)
        {
            return null;
        }

        public IEnumerable<Salaire> GetSalaireCadre(int idCadre, DateTime mois)
        {
            return null;
        }
    }
}