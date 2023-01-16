import style from './Home.module.css';
import { useNavigate } from 'react-router-dom';

function Home(props) {

    // HOOKS *************************************************************************************

    // Hook for navigating on the app
    let navigate = useNavigate();

    // RENDERING *********************************************************************************
    
    return (
        <section className={style.homesection}>
        <div>
            <h1>¥€$!</h1>
            <h3>La loterie sans se ruiner</h3>
        </div>            
        <div style={props.currentDrawClosed ? {color:"#FA8F89"} : {color:"white"}}>
            <h2>Jouez</h2>
            <h4>Le prochain tirage se tiendra dans</h4>
            <span id={style["counter"]}>{props.timer}</span>
            <button disabled={props.currentDrawClosed} className={style.homebutton} onClick={() => navigate('/play')}>Jouer</button>
        </div>
        <div>
            <h2>Vérifiez votre résultat</h2>
            <h4>Saisissez votre code</h4>
            <div>
                <div>
                    <span className='errorModal'>{props.errorMessage}</span>
                </div>
                <input type="text" maxLength="22" placeholder="Saisissez votre code" onChange={props.setAccessCode} value={props.accessCode} />
                <button onClick={props.navToResult} disabled={props.accessCode.length !== 22}>Vérifier</button>
            </div>
        </div>
    </section>
    );
}

export default Home;