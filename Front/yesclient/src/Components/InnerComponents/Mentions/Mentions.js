import style from './Mentions.module.css';

function Mentions() {

    // RENDERING *********************************************************************************

    return (
        <section className={style.mentions}>
            <h2>Mentions légales</h2>
            <div>
                <div>
                    <h3>Editeur :</h3>
                    <div>
                        <p>Nom de l’éditeur : CCI Campus Alsace Strasbourg<br/>
                        Date de création du site : Novembre 2022<br/>
                        Siret : 13002267600030    Adresse : 234 Av. de Colmar, 67021 Strasbourg, FRANCE<br/>
                        Téléphone : +33 (0) 3 68 67 20 00<br/>
                        Site web : www.ccicampus.fr</p>
                    </div>
                </div>
                <div>
                    <h3>Hébergement :</h3>
                    <div>
                        <p>Nom de l’hébergeur pour le site internet : Planet Hoster<br/>
                        Raison sociale : PlanetHoster<br/>
                        Adresse : 4416 Rue Louis B. Mayer, Laval, QUEBEC H7P 0G1, CANADA<br/>
                        Téléphone : +33 (0)1 76 60 41 43<br/>
                        Site web : www.planethoster.net</p>
                    </div>
                </div>
                <div>
                    <h3>Concepteur :</h3>
                    <div>
                        <p>Nom du concepteur : Thomas DE OLIVEIRA E SILVA<br/>
                        Cadre de la conception : Projet d’étude de formation de niveau BAC +3 de Concepteur Développeur d’Applications réalisé selon un énnoncé.</p>
                    </div>
                </div>
                <div>
                    <h3>Gains :</h3>
                    <div>
                        <p>L’ensemble des gains qu’il est possible de gagner sur ce site sont appelés “chou”.<br/>
                        Ces derniers sont purement fictifs et n’ont aucune valeur monétaire.</p>
                    </div>
                </div>
                <div>
                    <h3>Collecte de données :</h3>
                    <div>
                        <p>Aucune collecte de données n’est effectuée par the-yeslottery.com : il n’est pas nécessaire de fournir des données personnelles pour participer.<br/>
                        Ce site n’utilise pas de système de cookies.</p>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default Mentions;