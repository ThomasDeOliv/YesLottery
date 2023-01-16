import style from './Rules.module.css';

function Rule(props) {

    // RENDERING *********************************************************************************

    return (
        <section className={style.rules}>
            <h2>Règlement</h2>
            <div>
                <div>
                    <><p>Toutes les 5 minutes se tiendra un tirage aléatoire de 6 numéros compris entre 1 et 49.</p></>
                    <div>
                        <p>La cagnotte minimale de départ pour chaque tirage est de 10 choux.</p>
                        <p>Chaque joueur qui remplit une grille pour ce tirage apportera un chou supplémentaire à la cagnotte en cours.</p>
                        <p>Par exemple : si, pour un tirage, 6 joueurs ont rempli une grille, alors la cagnotte finale sera donc de 16 choux.</p>
                    </div>            
                    <div><p>Le jeu est anonyme et chaque joueur qui remplit une grille pour un tirage donné se verra remettre un code unique qui lui permettra de consulter le résultat de ce tirage, ainsi que ses gains éventuels. Il convient à chaque joueur de noter le code qui lui est remis.</p></div>
                    <div><p>Le calcul des gains se fait comme suit :</p> </div>
                    <div>
                        <ul>
                            <li>Le premier rang (6 bons numéros) se partage 60% de la cagnotte. </li>
                            <li>Le deuxième rang (5 bons numéros) se partage 20% de la cagnotte. </li>
                            <li>Le troisième rang (4 bons numéros) se partage 20% de la cagnotte.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </section>
    );
}

export default Rule;